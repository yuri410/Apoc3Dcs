﻿using System;
using System.Collections.Generic;
using System.Text;
using MainLogic;
using SlimDX;
using SlimDX.Direct3D9;
using VirtualBicycle.Input;
using VirtualBicycle.Logic.Goal;
using VirtualBicycle.Logic.PathWay;
using VirtualBicycle.Logic.Traffic;
using VirtualBicycle.MathLib;
using VirtualBicycle.ObjectMgr;
using VirtualBicycle.Physics;
using VirtualBicycle.Physics.Dynamics;
using VirtualBicycle.Scene;
using PM = VirtualBicycle.Physics.MathLib;
using VirtualBicycle.UI;

namespace VirtualBicycle.Logic.Competition
{
    /// <summary>
    /// 测试代码放在这儿
    /// </summary>
    public class TestCompetition : BaseCompetition
    {
        BicycleManager bicycleManager;

        Bicycle playerBicycle;
        //Bicycle comBicycle;
        Bicycle[] comBicycles;

        ObjectManager objectManager;
        ObjectCreator objectCreator;

        AIPathManager aiPathManager;
        GameMainLogic logic;
        Building destination;

        Vector3[] tempTest;

        public TestCompetition(Game game, GameMainLogic logic, World world, List<LogicalArea> areas, SceneObject[] sceneObjects)
            : base(game, world, areas, sceneObjects)
        {
            this.logic = logic;
        }

        private Bicycle AddComputerBicycle(Vector3 position, BicycleColor color) 
        {
            Bicycle bike = objectCreator.CreateBicycle(position, color);
            bike.BuildPhysicsModel(world.PhysicsWorld);
            bike.BicycleGoalMgr = new GoalManager(bike);
            bike.OwnerType = Bicycle.BicycleOwner.Computer;
            bicycleManager.RegisterBicycle(bike);
            return bike;
        }

        public override void Initialize()
        {
            #region 建立Bicycle和道路信息
            bicycleManager = new BicycleManager();
            objectManager = new ObjectManager();
            objectCreator = new ObjectCreator(device, game.CurrentWorld, objectManager);

            playerBicycle = objectCreator.CreateBicycle(new Vector3(2, 2, 2), BicycleColor.Red);
            playerBicycle.OwnerType = Bicycle.BicycleOwner.Player;
            playerBicycle.BuildPhysicsModel(world.PhysicsWorld);
            bicycleManager.RegisterBicycle(playerBicycle);

            //把当前的目标设置到CurrentBicycle上
            CurrentBicycle = playerBicycle;

            comBicycles = new Bicycle[3];

            comBicycles[0] = AddComputerBicycle(new Vector3(10, 2, 10), BicycleColor.Green);
            comBicycles[1] = AddComputerBicycle(new Vector3(20, 2, 20), BicycleColor.Yellow);
            comBicycles[2] = AddComputerBicycle(new Vector3(30, 2, 30), BicycleColor.Purple);

            //comBicycle = objectCreator.CreateBicycle(new Vector3(10, 2, 10));
            //comBicycle.BuildPhysicsModel(world.PhysicsWorld);
            //comBicycle.BicycleGoalMgr = new GoalManager(comBicycle);
            //comBicycle.OwnerType = Bicycle.BicycleOwner.Computer;
            //bicycleManager.RegisterBicycle(comBicycle);


            //建立道路信息
            aiPathManager = new AIPathManager(SceneObjects);
            Queue<Vector3> points = new Queue<Vector3>();

            if (aiPathManager.AITrafficComponets.Count > 0)
            {
                AIPort comBicycleStartPort = aiPathManager.GetPortOfUID(0);
                //comBicycle.Position = comBicycleStartPort.Position + Vector3.UnitY * 2;
                //comBicycle.Orientation = Quaternion.RotationAxis(Vector3.UnitY, MathEx.PIf);

                for (int i = 0; i < comBicycles.Length; i++)
                {
                    comBicycles[i].Position = comBicycleStartPort.Position + Vector3.UnitY * 2;
                    comBicycles[i].Orientation = Quaternion.RotationAxis(Vector3.UnitY, MathEx.PIf);

                    GoalManager goalMgr = comBicycles[i].BicycleGoalMgr;
                    BicycleThinkGoal thinkGoal = new BicycleThinkGoal(comBicycles[i], goalMgr, null);

                    thinkGoal.AddSubGoal(new BicycleToPorts(comBicycles[i], goalMgr, thinkGoal, aiPathManager, 0, aiPathManager.AIPorts.Count - 1));
                    goalMgr.GoalSeq.Enqueue(thinkGoal);
                }


                playerBicycle.Position = comBicycleStartPort.Position + new Vector3(2f, 2f, 2f);
                playerBicycle.Orientation = Quaternion.RotationAxis(Vector3.UnitY, MathEx.PIf);
            }
            #endregion

            #region 初始化摄像机类
            playerView = new ViewManager((float)game.Window.ClientSize.Width / (float)game.Window.ClientSize.Height);

            playerView.SetNearPlane(0.1f);
            playerView.SetFarPlane(750);
            playerView.SetFieldOfView(45);
            playerView.SetRenderTarget(device.GetRenderTarget(0));
            playerView.Mode = ViewMode.ThirdPersion;
            playerView.CurrentBicycle = playerBicycle;

            world.Scene.RegisterCamera(playerView);

            InputManager.Instance.ViewChanged += this.ViewChanged;
            InputManager.Instance.Reset += this.ResetPlayerBicycle;
            
            #endregion

            
            for (int i = 0; i < base.SceneObjects.Length; i++) 
            {
                if (destination == null)
                {
                    Building bld = SceneObjects[i] as Building;
                    if (bld != null)
                    {
                        if (CaseInsensitiveStringComparer.Compare(bld.TypeName, "finalflag"))
                        {
                            destination = bld;
                        }
                    }
                }
                if (tempTest == null)
                {
                    Road road = SceneObjects[i] as Road;

                    if (road != null) 
                    {
                        List<RoadNode> nodes = road.TrackLine.InterposedPoints;

                        tempTest = new Vector3[nodes.Count];

                        for (int j = 0; j < nodes.Count; j++)
                        {
                            tempTest[j] = nodes[j].Position;
                        }

                        Vector3 dir3 = tempTest[1] - tempTest[0];
                        Vector2 dir = new Vector2(dir3.X, dir3.Z);
                        dir.Normalize();

                        Quaternion ori = Quaternion.RotationAxis(Vector3.UnitY, MathEx.Vector2DirAngle(dir));

                        playerBicycle.Orientation = ori;
                        playerBicycle.Position = tempTest[0] + Vector3.UnitY * 2;

                        for (int j = 0; j < comBicycles.Length; j++)
                        {
                            comBicycles[j].Orientation = ori;

                            if (j + 1 < tempTest.Length)
                            {
                                comBicycles[j].Position = tempTest[j + 1] + 2 * Vector3.UnitY;
                            }
                            else
                            {
                                comBicycles[j].Position = tempTest[tempTest.Length - 1] + (j + 3 - tempTest.Length) * Vector3.UnitY;
                            }
                        }
                        ResetPlayerBicycle(this, EventArgs.Empty);
                    }
                }
                if (destination != null && tempTest != null) 
                {
                    break;
                }
            }

            base.Initialize();
        }

        void ResetBicycle(Bicycle bike)
        {
            if (tempTest != null)
            {
                float distance = float.MaxValue;
                int nearestIndex = -1;

                Vector3 pos = bike.Position;
                for (int i = 0; i < tempTest.Length; i++)
                {
                    float d = Vector3.Distance(pos, tempTest[i]);

                    if (d < distance)
                    {
                        distance = d;
                        nearestIndex = i;
                    }
                }

                if (nearestIndex != -1)
                {
                    bike.Position = tempTest[nearestIndex] + Vector3.UnitY * 2;

                    if (nearestIndex < tempTest.Length - 1)
                    {
                        Vector3 dir3 = tempTest[nearestIndex + 1] - tempTest[nearestIndex];
                        Vector2 dir = new Vector2(dir3.X, dir3.Z);
                        dir.Normalize();

                        Quaternion ori = Quaternion.RotationAxis(Vector3.UnitY, MathEx.Vector2DirAngle(dir));
                        bike.Orientation = ori;

                        bike.RigidBody.LinearVelocity = Vector3.Zero;
                        bike.RigidBody.AngularVelocity = Vector3.Zero;

                    }
                }
            }

        }
        protected void ResetPlayerBicycle(object sender, EventArgs e)
        {
            ResetBicycle(playerBicycle);
        }

        bool gameIsOver;
        void Win()
        {
            if (!gameIsOver)
            {
                IngameUI igui = game.GameUI.CurrentComponent as IngameUI;

                playerView.IsFreeze = true;
                playerView.Mode = ViewMode.ThirdPersion;

                if (igui != null)
                {
                    igui.IsFinishedUIShown = true;
                }
                gameIsOver = true;
            }
        }

        public override void Update(float dt)
        {
            if (world != null && world.IsValid)
            {
                RigidBody body;
                for (int i = 0; i < comBicycles.Length; i++) 
                {
                    body = comBicycles[i].RigidBody;
                    DefaultMotionState motionState = (DefaultMotionState)body.MotionState;
                    PM.Matrix worldTransform = motionState.GraphicsWorldTransform;

                    comBicycles[i].UpdateLogic(dt);

                    if (!body.IsActive)
                        body.Activate();

                    if (destination != null)
                    {
                        float dist = Vector3.Distance(comBicycles[i].BoundingSphere.Center, destination.BoundingSphere.Center);
                        if (dist < 6)
                        {
                            CannotWin = true;
                        }
                    }
                }


                playerBicycle.UpdateLogic(dt);

                body = playerBicycle.RigidBody;

                if (!body.IsActive)
                    body.Activate();

                if (destination != null && !CannotWin)
                {
                    float dist = Vector3.Distance(playerBicycle.BoundingSphere.Center, destination.BoundingSphere.Center);

                    if (dist < 6)
                    {
                        Win();
                    }
                }
            }

            base.Update(dt);
        }

        protected override void Dispose(bool disposing)
        {
            Unload();
            playerBicycle.Dispose();

            for (int i = 0; i < comBicycles.Length; i++)
            {
                comBicycles[i].Dispose();
            }
        }

        public override void Unload()
        {
            InputManager.Instance.ViewChanged -= this.ViewChanged;
            InputManager.Instance.Reset -= this.ResetPlayerBicycle;
            
            base.Unload();
        }

        protected void ViewChanged(object sender, EventArgs e)
        {
            if (world != null && world.IsValid)
            {
                if (playerView.Mode == ViewMode.FirstPerson)
                {
                    playerView.Mode = ViewMode.ThirdPersion;
                }
                else
                {
                    playerView.Mode = ViewMode.FirstPerson;
                }
            }
        }
    }
}
