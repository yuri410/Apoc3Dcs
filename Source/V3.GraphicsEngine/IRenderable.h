#pragma once

namespace V3
{
	namespace GraphicsEngine
	{
		/// <summary>
		///  ����һ��ͨ�� ��ȡ��Ⱦ����(RenderOperation) �ķ�ʽ ��һ�ֱ��ڹ������Ⱦ��ʽ
		/// </summary>
		public interface struct IRenderable
		{
	        /// <summary>
			///  �����Ⱦ����
			/// </summary>
			/// <returns></returns>
			virtual array<RenderOperation>^ GetRenderOperation() = 0;
		};
	}
}
