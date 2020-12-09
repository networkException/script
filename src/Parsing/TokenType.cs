namespace networkScript.Parsing {
	public enum TokenType {
		For,
		While,
		If,
		Else,
		ParenOpen,
		ParenClose,
		CurlyOpen,
		CurlyClose,
		BracketOpen,
		BracketClose,
		TemplateOpen,
		TemplateClose,
		Let,
		Const,
		Class,
		Equals,
		Semicolon,
		Comma,
		Period,
		Asterisk,
		Slash,
		Minus,
		Greater,
		Less,
		PlusPlus,
		PlusEquals,
		EqualsEquals,
		ExclamationEquals,
		ColumnEquals,
		Plus,
		Identifier,
		StringLiteral,
		BooleanLiteral,
		DoubleLiteral,
		IntLiteral,
		TypeDefinition,
		Eof
	}

	public enum StringToken {
		None,
		SingleQuote,
		DoubleQuote,
		Template
	}
}