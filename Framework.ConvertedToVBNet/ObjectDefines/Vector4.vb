Namespace ObjectDefines
	Public Structure Vector4
		Public Property X() As Single
			Get
				Return m_X
			End Get
			Set
				m_X = Value
			End Set
		End Property
		Private m_X As Single
		Public Property Y() As Single
			Get
				Return m_Y
			End Get
			Set
				m_Y = Value
			End Set
		End Property
		Private m_Y As Single
		Public Property Z() As Single
			Get
				Return m_Z
			End Get
			Set
				m_Z = Value
			End Set
		End Property
		Private m_Z As Single
		Public Property O() As Single
			Get
				Return m_O
			End Get
			Set
				m_O = Value
			End Set
		End Property
		Private m_O As Single
	End Structure
End Namespace
