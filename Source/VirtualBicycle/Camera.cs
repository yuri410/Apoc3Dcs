using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using VirtualBicycle.MathLib;

namespace VirtualBicycle
{
    public interface ICamera 
    {
        Frustum Frustum { get; }

        Matrix ProjectionMatrix { get; }
        Matrix ViewMatrix { get; }

        void Update(float dt);

        Vector3 Position { get; }

        Quaternion Orientation { get; }

        float FarPlane { get; }
        float NearPlane { get; }
        float AspectRatio { get; }
        float FieldOfView { get; }

        Vector3 Front { get; }
        Vector3 Top { get; }
        Vector3 Right { get; }

        Surface RenderTarget { get; }
    }


    /// <summary>
    /// Represents a view onto a 3D scene.
    /// </summary>
    public class Camera : ICamera
    {
        #region Fields and Properties
        protected Vector3 position;

        protected Quaternion orientation;

        protected Vector3 front;

        protected Vector3 top;

        protected Vector3 right;

        float fovy;
        float near;
        float far;
        float aspect;

        Frustum frustum = new Frustum();

        protected bool isProjDirty;

        public Surface RenderTarget
        {
            get;
            set;
        }
        public Frustum Frustum
        {
            get { return frustum; }
        }

        /// <summary>
        /// Gets the view direction(AKA z axis in camera space)
        /// ��ȡ������ĳ���������ռ�Z�ᣩ
        /// </summary>
        public Vector3 Front
        {
            get { return front; }
        }

        /// <summary>
        /// �����Y
        /// </summary>
        public Vector3 Top
        {
            get { return top; }
        }

        /// <summary>
        /// �����X
        /// </summary>
        public Vector3 Right
        {
            get { return right; }
        }

        /// <summary>
        /// Gets or sets the position of the camera eye point.
        /// ��ȡ�������Ե��λ��
        /// </summary>
        /// <value>
        /// The position of the camera eye point.
        /// �ӵ��λ��
        /// </value>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Quaternion Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public float FieldOfView
        {
            get { return MathEx.Radian2Degree(fovy); }
            set
            {
                fovy = MathEx.Degree2Radian(value);
                isProjDirty = true;
            }
        }

        public float NearPlane
        {
            get { return near; }
            set
            {
                near = value;
                isProjDirty = true;
            }
        }

        public float FarPlane
        {
            get { return far; }
            set
            {
                far = value;
                isProjDirty = true;
            }
        }

        public float AspectRatio
        {
            get { return aspect; }
            set
            {
                aspect = value;
                isProjDirty = true;
            }
        }

        public Matrix ViewMatrix
        {
            get { return frustum.view; }
            protected set { frustum.view = value; }
        }
        public Matrix ProjectionMatrix
        {
            get { return frustum.proj; }
            protected set { frustum.proj = value; }
        }

        public float NearPlaneWidth
        {
            get;
            protected set;
        }
        public float NearPlaneHeight
        {
            get;
            protected set;
        }
        #endregion

        public Camera(float aspect)
        {
            FarPlane = 200;
            FieldOfView = 45;
            NearPlane = 0.1f;
            AspectRatio = aspect;
            orientation = Quaternion.Identity;
            Update(0);
        }

        #region ����

        public virtual void GetSubareaProjection(ref RectangleF rect, out Matrix mat)
        {
            //Matrix.PerspectiveRH(rect.Width, rect.Height, NearPlane, FarPlane, out mat);           
            Matrix.PerspectiveOffCenterRH(rect.Left * NearPlaneWidth, rect.Right * NearPlaneWidth, rect.Bottom * NearPlaneHeight, rect.Top * NearPlaneHeight, NearPlane, FarPlane, out mat);
        }

        public virtual void UpdateProjection()
        {
            //fFrustum.proj = Matrix.PerspectiveFovRH(MathEx.Angle2Radian(dFovy), dAspect, dNear, dFar);

            //float radFov = MathEx.Radian2Angle(dFovy);


            NearPlaneHeight = (float)(Math.Tan(fovy * 0.5f)) * NearPlane * 2;
            NearPlaneWidth = NearPlaneHeight * AspectRatio;


            frustum.proj = Matrix.PerspectiveRH(NearPlaneWidth, NearPlaneHeight, near, far);

            isProjDirty = false;
        }

        public virtual void Update(float dt)
        {
            //Vector3 vMiPos = -vPosition;
            //mViewMatrix=//.LoadTranslateMatrix(ref vMiPos);
            //mViewMatrix *= qOri.ToMatrix4(new Vector3());

            //mProjMatrix.LoadProjectionMatrix(dFovy, dAspect, dNear, dFar);

            //�����Ҫ����Projection Matrix
            if (isProjDirty)
            {
                UpdateProjection();
            }

            //�����������Front,Top,Right����

            //frustum.view = Matrix.RotationQuaternion(orientation);// Matrix.LookAtRH(position, position + front, top);

            Matrix m = Matrix.RotationQuaternion(orientation);

            front = MathEx.GetMatrixFront(ref m); // MathEx.QuaternionRotate(orientation, new Vector3(0, 0, 1));
            top = MathEx.GetMatrixUp(ref m);      // MathEx.QuaternionRotate(orientation, new Vector3(0, 1, 0));
            right = MathEx.GetMatrixRight(ref m);  // MathEx.QuaternionRotate(orientation, new Vector3(1, 0, 0));

            frustum.view = Matrix.LookAtRH(position, position + front, top);
            //MathEx.QuaternionToMatrix(ref orientation, out frustum.view);
            
            //frustum.view = Matrix.Translation(-position) * frustum.view;
            frustum.Update();
        }

        public virtual void ResetView() { }

        #endregion
    }

    /// <summary>
    /// ��һ�˳������,�ṩ��6���ɶȵĲ���
    /// </summary>
    public class FpsCamera : Camera
    {
        #region �ֶ�

        protected float dMoveSpeed = 2.5f;
        protected float turnSpeed = MathEx.PIf / 45;

        #endregion

        public FpsCamera(float aspect)
            : base(aspect)
        { }

        #region ����

        public float MoveSpeed
        {
            get { return dMoveSpeed; }
            set { dMoveSpeed = value; }
        }
        public float TurnSpeed
        {
            get { return turnSpeed; }
            set { turnSpeed = value; }
        }

        #endregion

        #region ����

        public void MoveAbs(Vector3 mov)
        {
            position += MathEx.QuaternionRotate(orientation, mov);
        }

        public void MoveFront()
        {
            position += front * dMoveSpeed;
        }
        public void MoveBack()
        {
            position -= front * dMoveSpeed;
        }
        public void MoveLeft()
        {
            position -= right * dMoveSpeed;
        }
        public void MoveRight()
        {
            position += right * dMoveSpeed;
        }
        public void MoveUp()
        {
            position += top * dMoveSpeed;
        }
        public void MoveDown()
        {
            position -= top * dMoveSpeed;
        }

        public void TurnLeft()
        {
            Quaternion iq = orientation;
            iq.Conjugate();
            Vector3 top = MathEx.QuaternionRotate(iq, new Vector3(0, 1, 0));

            orientation *= Quaternion.RotationAxis(top, -turnSpeed);
            //qOri *= Quaternion.FromAngleAxis(dTurnSpeed, new Vector3(0, -1, 0));
            orientation.Normalize();
        }
        public void TurnRight()
        {
            Quaternion iq = orientation;
            iq.Conjugate();

            Vector3 top = MathEx.QuaternionRotate(iq, new Vector3(0, 1, 0));// (~qOri).Rotate(new Vector3(0, 1, 0));

            orientation *= Quaternion.RotationAxis(top, turnSpeed);
            orientation.Normalize();
        }
        public void TurnUp()
        {
            orientation *= Quaternion.RotationAxis(new Vector3(1, 0, 0), -turnSpeed);
            orientation.Normalize();
        }
        public void TurnDown()
        {
            orientation *= Quaternion.RotationAxis(new Vector3(1, 0, 0), turnSpeed);
            orientation.Normalize();
        }
        public void RollLeft()
        {
            orientation *= Quaternion.RotationAxis(new Vector3(0, 0, 1), turnSpeed);
            orientation.Normalize();
        }
        public void RollRight()
        {
            orientation *= Quaternion.RotationAxis(new Vector3(0, 0, -1), turnSpeed);
            orientation.Normalize();
        }

        #endregion
    }

    public class ChaseCamera : Camera
    {
        public ChaseCamera(float acpectRatio)
            : base(acpectRatio)
        {
            top = new Vector3(0, 1, 0);
        }

        #region Chased object properties (set externally each frame)

        /// <summary>
        /// Position of object being chased.
        /// </summary>
        public Vector3 ChasePosition
        {
            get { return chasePosition; }
            set { chasePosition = value; }
        }
        private Vector3 chasePosition;

        /// <summary>
        /// Direction the chased object is facing.
        /// </summary>
        public Vector3 ChaseDirection
        {
            get { return chaseDirection; }
            set { chaseDirection = value; }
        }
        private Vector3 chaseDirection = Vector3.UnitZ;

        #endregion

        #region Desired camera positioning (set when creating camera or changing view)

        /// <summary>
        /// Desired camera position in the chased object's coordinate system.
        /// </summary>
        public Vector3 DesiredPositionOffset
        {
            get { return desiredPositionOffset; }
            set { desiredPositionOffset = value; }
        }
        private Vector3 desiredPositionOffset = new Vector3(0, 1.2f, 4);

        /// <summary>
        /// Desired camera position in world space.
        /// </summary>
        public Vector3 DesiredPosition
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return desiredPosition;
            }
        }
        private Vector3 desiredPosition;

        /// <summary>
        /// Look at point in the chased object's coordinate system.
        /// </summary>
        public Vector3 LookAtOffset
        {
            get { return lookAtOffset; }
            set { lookAtOffset = value; }
        }
        private Vector3 lookAtOffset = new Vector3(0, 0, 0);

        /// <summary>
        /// Look at point in world space.
        /// </summary>
        public Vector3 LookAt
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return lookAt;
            }
        }
        private Vector3 lookAt;

        #endregion

        #region Camera physics (typically set when creating camera)

        /// <summary>
        /// Physics coefficient which controls the influence of the camera's position
        /// over the spring force. The stiffer the spring, the closer it will stay to
        /// the chased object.
        /// </summary>
        public float Stiffness
        {
            get { return stiffness; }
            set { stiffness = value; }
        }
        private float stiffness = 1800.0f;

        /// <summary>
        /// Physics coefficient which approximates internal friction of the spring.
        /// Sufficient damping will prevent the spring from oscillating infinitely.
        /// </summary>
        public float Damping
        {
            get { return damping; }
            set { damping = value; }
        }
        private float damping = 600.0f;

        /// <summary>
        /// Mass of the camera body. Heaver objects require stiffer springs with less
        /// damping to move at the same rate as lighter objects.
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        private float mass = 50.0f;

        #endregion

        #region Current camera properties (updated by camera physics)
        /// <summary>
        /// Velocity of camera.
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
        }
        private Vector3 velocity;

        #endregion


        #region Methods

        /// <summary>
        /// Rebuilds object space values in world space. Invoke before publicly
        /// returning or privately accessing world space values.
        /// </summary>
        private void UpdateWorldPositions()
        {
            // Construct a matrix to transform from object space to worldspace
            Matrix transform = Matrix.Identity;
            //transform.Forward = ChaseDirection;
            //transform.Up = Up;
            right = Vector3.Cross(top, chaseDirection);
            right.Normalize();

            //Vector3 vec = chaseDirection;

            front = chaseDirection;

            transform.M31 = -chaseDirection.X;
            transform.M32 = -chaseDirection.Y;
            transform.M33 = -chaseDirection.Z;

            transform.M21 = top.X;
            transform.M22 = top.Y;
            transform.M23 = top.Z;

            transform.M11 = right.X;
            transform.M12 = right.Y;
            transform.M13 = right.Z;

            // Calculate desired camera properties in world space
            desiredPosition = ChasePosition +
                Vector3.TransformNormal(DesiredPositionOffset, transform);
            lookAt = ChasePosition +
                Vector3.TransformNormal(LookAtOffset, transform);
        }

        /// <summary>
        /// Rebuilds camera's view and projection matricies.
        /// </summary>
        private void UpdateMatrices()
        {
            Frustum.view = Matrix.LookAtRH(this.Position, this.LookAt, this.top);
            //projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,
            //    AspectRatio, NearPlaneDistance, FarPlaneDistance);
        }

        /// <summary>
        /// Forces camera to be at desired position and to stop moving. The is useful
        /// when the chased object is first created or after it has been teleported.
        /// Failing to call this after a large change to the chased object's position
        /// will result in the camera quickly flying across the world.
        /// </summary>
        public override void ResetView()
        {
            UpdateWorldPositions();

            // Stop motion
            velocity = Vector3.Zero;

            // Force desired position
            position = desiredPosition;

            UpdateMatrices();
        }

        /// <summary>
        /// Animates the camera from its current position towards the desired offset
        /// behind the chased object. The camera's animation is controlled by a simple
        /// physical spring attached to the camera and anchored to the desired position.
        /// </summary>
        public override void Update(float dt)
        {
            UpdateWorldPositions();

            // Calculate spring force
            Vector3 stretch = position - desiredPosition;
            Vector3 force = -stiffness * stretch - damping * velocity;

            // Apply acceleration
            Vector3 acceleration = force / mass;
            velocity += acceleration * dt;

            // Apply velocity
            position += velocity * dt;

            UpdateMatrices();

            if (isProjDirty)
            {
                UpdateProjection();
            }

            //MathEx.QuaternionToMatrix(ref orientation, out Frustum.view);


            Frustum.Update();//ref mViewMatrix, ref mProjMatrix);

            //�����������Front,Top,Right����

            //front = new Vector3(0, 0, -1);
            //top = new Vector3(0, 1, 0);
            //right = new Vector3(1, 0, 0);

            //MathEx.MatrixTransformVec(ref Frustum.view, ref front);
            //MathEx.MatrixTransformVec(ref Frustum.view, ref top);
            //MathEx.MatrixTransformVec(ref Frustum.view, ref right);



            //front = MathEx.QuaternionRotate(orientation, new Vector3(0, 0, -1));
            //top = MathEx.QuaternionRotate(orientation, new Vector3(0, 1, 0));
            //right = MathEx.QuaternionRotate(orientation, new Vector3(1, 0, 0));
        }

        #endregion
    }
}
