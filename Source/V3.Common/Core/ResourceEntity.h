#pragma once

#include "ResourceManager.h"

using namespace System;
using namespace System::ComponentModel;


namespace V3
{
	namespace Core
	{
		/// <summary>
		///  ����һ����Դ��״̬
		/// </summary>
		public enum ResourceState
		{
			/// <summary>
			///  δ����
			/// </summary>
			Unloaded,
			/// <summary>
			///  ���ڼ���
			/// </summary>
			Loading,        
			/// <summary>
			///  �Ѿ�����
			/// </summary>
			Loaded,
			/// <summary>
			///  ����ж��
			/// </summary>
			Unloading
		};


		/// <summary>
		///  ��ʾһ����Դ��������ģ�͵�
		/// </summary>
		/// <remarks>
		/// ��Դ�����֣����ú�ʵ��
		/// ������������Դʵ��
		/// ���ò��ܹ�����ʹ��ʱ��Ӱ��ʵ���״̬
		/// ��Դ����������ʵ����Դ��״̬������й���
		/// </remarks>
		public ref class ResourceEntity abstract
		{
		private:
			ResourceState m_resState;
			TimeSpan m_creationTime;
			TimeSpan m_lastAccessed;
			int m_accessTimes;
			int m_referenceCount;
			
			bool m_isUnmanaged;

			float m_useFrequency;

			ResourceManager^ m_manager;

			String^ m_hashString;


		protected:
			ResourceEntity()
				: m_isUnmanaged(true)
			{

			}

			ResourceEntity(ResourceManager^ manager, String^ hashString) : m_hashString(hashString),
																		  m_manager(manager)	
			{
				AllowDynamicLoading = true;
			}

			ResourceEntity::ResourceEntity(ResourceManager^ manager, String^ hashString, bool allowdl) : m_hashString(hashString),
																										 m_manager(manager)
			{
				AllowDynamicLoading = allowdl;
			}

		public:

			/// <summary>
			///  ��ȡһ��System.Boolean����ʾ����Դ�Ƿ�����̬����/ж��
			/// </summary>
			[BrowsableAttribute(false)]
			property bool AllowDynamicLoading
			{
				bool get();
				void set(bool value);
			}
			/// <summary>
			///  ��ȡ����Դ�Ĵ���ʱ��
			/// </summary>
			[BrowsableAttribute(false)]
			property TimeSpan CreationTime
			{
				TimeSpan get() { return m_creationTime; }
			}

			/// <summary>
			///  ��ȡ�ϴη��ʸ���Դ��ʱ��
			/// </summary>
			[BrowsableAttribute(false)]
			property TimeSpan LastAccessedTime
			{
				TimeSpan get() { return m_lastAccessed; }
			}

			/// <summary>
			///  ��ù������Դ����Դ������
			/// </summary>
			[BrowsableAttribute(false)]
			property ResourceManager^ Manager
			{
				ResourceManager^ get() { return m_manager; }
			}

			/// <summary>
			///  ��ȡ����Դ��ʹ��Ƶ��
			/// </summary>
			[BrowsableAttribute(false)]
			property float UseFrequency
			{
				float get() { return m_useFrequency; }
			}

			/// <summary>
			/// ��ȡ��Դʵ������ü���
			/// </summary>
			[BrowsableAttribute(false)]
			property int ReferenceCount
			{
				int get() { return m_referenceCount; }
				void set(int value) { m_referenceCount = value; }
			}

			[BrowsableAttribute(false)]
			property String^ HashString
			{
				String^ get() { return m_hashString; }
			}

			void Use(void);
			void Load(void);
			void Unload(void);
			

		};
	}
}