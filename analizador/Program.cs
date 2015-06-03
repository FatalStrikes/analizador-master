using System;
using System.IO;

namespace analizador
{
	class MainClass
	{
//		public Token[] palabrasReservadas = {
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_THEN, "then"),
//			new Token (TknType.TKN_ELSE, "else"),
//			new Token (TknType.TKN_FI, "fi"),
//			new Token (TknType.TKN_DO, "do"),
//			new Token (TknType.TKN_UNTIL, "until"),
//			new Token (TknType.TKN_WHILE, "while"),
//			new Token (TknType.TKN_READ, "read"),
//			new Token (TknType.TKN_WRITE, "write"),
//			new Token (TknType.TKN_FLOAT, "float"),
//			new Token (TknType.TKN_INT, "int"),
//			new Token (TknType.TKN_BOOL, "bool"),
//			new Token (TknType.TKN_PROGRAM, "program"),
//			new Token (TknType.TKN_AND, "and"),
//			new Token (TknType.TKN_OR, "or"),
//			new Token (TknType.TKN_NOT, "not"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//			new Token (TknType.TKN_IF, "if"),
//		};

		static public Token getToken(string code){
			buffer = code.ToCharArray();
			curState = State.IN_START;
			Token token = new Token();
			int tokIndex = 0;

			if(isEOF()){
				return new Token(TknType.TKN_EOF, "");
			}

			// DOES ALL THE HEAVY-LIFTING
			while (curState != State.IN_DONE) {
				switch(curState){

				case State.IN_START:
					if (Char.IsControl (buffer [bufIndex]) || Char.IsWhiteSpace(buffer [bufIndex])) {
						curState = State.IN_DONE;
						break;
					}else if(buffer[bufIndex] == '{'){
						curState = State.IN_LBRACKET;
					}else if(buffer[bufIndex] == '}'){
						curState = State.IN_RBRACKET;
					}else if(buffer[bufIndex] == '('){
						curState = State.IN_LPAREN;
					}else if(buffer[bufIndex] == ')'){
						curState = State.IN_RPAREN;
					}else if(buffer[bufIndex] == '+'){
						curState = State.IN_ADD;
					}else if(buffer[bufIndex] == '-'){
						curState = State.IN_MINUS;
					}else if(buffer[bufIndex] == '>'){
						curState = State.IN_COMPARE;
					}else if(buffer[bufIndex] == '<'){
						curState = State.IN_COMPARE;
					}else if(buffer[bufIndex] == '!'){
						curState = State.IN_COMPARE;
					}else if(buffer[bufIndex] == '='){
						curState = State.IN_ASSIGN;
					}else if(buffer[bufIndex] == '*'){
						curState = State.IN_MULTI;
					}else if(buffer[bufIndex] == '/'){
						curState = State.IN_COMMENT;
					}else if(buffer[bufIndex] == ';'){
						curState = State.IN_SEMICOLON;
					}else if (Char.IsLetter (buffer [bufIndex])) {
						curState = State.IN_ID;
					} else if (Char.IsDigit (buffer [bufIndex])) {
						curState = State.IN_NUM;
					} else if (isEOF ()) {
						curState = State.IN_DONE;
						token.type = TknType.TKN_EOF;
						return token;
					}
					token.lexema [tokIndex] = buffer [bufIndex];
					tokIndex++;
					break;

				// IDENTIFY ALL ID TOKENS, ANYTHING THAT STARTS WITH A LETTER IS A ID UNTIL IDENTIFIED AS RESERVED WORD
				case State.IN_ID:
					if(! (Char.IsLetterOrDigit(buffer[bufIndex]) || buffer[bufIndex] == '_')){
						curState = State.IN_DONE;
						token.type = TknType.TKN_ID;
						token.lexema [tokIndex] = '\0';
						bufIndex--;
						break;
					}else if (isEOF ()) {
						curState = State.IN_DONE;
						token.type = TknType.TKN_EOF;
						bufIndex--;
						return token;
					}
					token.lexema [tokIndex] = buffer [bufIndex];
					tokIndex++;
					break;


				case State.IN_NUM:
					if (!Char.IsDigit (buffer [bufIndex]) && !buffer[bufIndex].Equals('.')) {
						curState = State.IN_DONE;
						token.type = TknType.TKN_NUM;
						bufIndex--;
						break;
					} else if (isEOF ()) {
						curState = State.IN_DONE;
						token.type = TknType.TKN_EOF;
						bufIndex--;
						return token;
					}
					token.lexema [tokIndex] = buffer [bufIndex];
					tokIndex++;
					break;


				case State.IN_LBRACKET:
					token.type = TknType.TKN_LBRACK;
					curState = State.IN_DONE;
					bufIndex--;
					break;
				case State.IN_RBRACKET:
					token.type = TknType.TKN_RBRACK;
					curState = State.IN_DONE;
					bufIndex--;
					break;


				case State.IN_LPAREN:
					token.type = TknType.TKN_LPAREN;
					curState = State.IN_DONE;
					bufIndex--;
					break;
				case State.IN_RPAREN:
					token.type = TknType.TKN_RPAREN;
					curState = State.IN_DONE;
					bufIndex--;
					break;

				case State.IN_ADD:
					token.type = TknType.TKN_ADD;
					curState = State.IN_DONE;
					bufIndex--;
					break;
				case State.IN_MINUS:
					token.type = TknType.TKN_MINUS;
					curState = State.IN_DONE;
					bufIndex--;
					break;
				case State.IN_MULTI:
					token.type = TknType.TKN_MULTI;
					curState = State.IN_DONE;
					bufIndex--;
					break;

				case State.IN_SEMICOLON:
					token.type = TknType.TKN_SEMICOLON;
					curState = State.IN_DONE;
					bufIndex--;
					break;

				case State.IN_COMPARE:
					if (buffer [bufIndex] != '=') {
						//token.lexema [tokIndex] = buffer [bufIndex];
						token.type = TknType.TKN_COMPARE;
						curState = State.IN_DONE;
						bufIndex--;
					} else {
						token.lexema [tokIndex] = buffer [bufIndex];
						token.type = TknType.TKN_COMPARE;
						curState = State.IN_DONE;
					}
					break;

				case State.IN_ASSIGN:
					if (buffer [bufIndex] == '=') {
						token.lexema [tokIndex] = buffer [bufIndex];
						token.type = TknType.TKN_COMPARE;
						curState = State.IN_DONE;
					} else {
						token.type = TknType.TKN_ASSIGN;
						curState = State.IN_DONE;
						bufIndex--;
					}

					break;

				case State.IN_COMMENT:
					if (buffer [bufIndex] == '/') {
						while (!Char.IsControl (buffer [bufIndex])) {
							token.lexema [tokIndex] = buffer [bufIndex];
							tokIndex++;
							bufIndex++;
						}
						curState = State.IN_DONE;
						break;
					} else {
						curState = State.IN_DONE;
						token.type = TknType.TKN_DIV;
						bufIndex--;
					}
					break;
				}

				// Next char! FOR DEBUG
//				 Console.WriteLine (new String(token.lexema) + "->" + curState);
				bufIndex++;
			}


			// IDENTIFY ANY TOKEN ID AND CHECK IF IT IS A RESERVED WORD
			if (token.type == TknType.TKN_ID) {
				if (String.Compare (new string (token.lexema), "int", false) == 0) {
					token.type = TknType.TKN_INT;
				} else if (String.Compare (new string (token.lexema), "program", false) == 0) {
					token.type = TknType.TKN_PROGRAM;
				} else if (String.Compare (new string (token.lexema), "float", false) == 0) {
					token.type = TknType.TKN_FLOAT;
				} else if (String.Compare (new string (token.lexema), "else", false) == 0) {
					token.type = TknType.TKN_ELSE;
				} else if (String.Compare (new string (token.lexema), "then", false) == 0) {
					token.type = TknType.TKN_THEN;
				} else if (String.Compare (new string (token.lexema), "fi", false) == 0) {
					token.type = TknType.TKN_FI;
				} else if (String.Compare (new string (token.lexema), "do", false) == 0) {
					token.type = TknType.TKN_DO;
				} else if (String.Compare (new string (token.lexema), "until", false) == 0) {
					token.type = TknType.TKN_UNTIL;
				} else if (String.Compare (new string (token.lexema), "read", false) == 0) {
					token.type = TknType.TKN_READ;
				} else if (String.Compare (new string (token.lexema), "write", false) == 0) {
					token.type = TknType.TKN_WRITE;
				} else if (String.Compare (new string (token.lexema), "bool", false) == 0) {
					token.type = TknType.TKN_BOOL;
				} else if (String.Compare (new string (token.lexema), "and", false) == 0) {
					token.type = TknType.TKN_AND;
				} else if (String.Compare (new string (token.lexema), "or", false) == 0) {
					token.type = TknType.TKN_OR;
				} else if (String.Compare (new string (token.lexema), "not", false) == 0) {
					token.type = TknType.TKN_NOT;
				}
			}

			return token;
		}


		// END OF FILE
		private static bool isEOF()
		{
			if(bufIndex == buffer.Length)	return true;
			return false;
		}

		// NOT USED?
		private static bool isDelim(char c)
		{
			char [] delim = {' ','\n','\t', '\r'};
			for(int i=0;i<3;i++)
			{
				if(c==delim[i])
					return true;
			}
			return false;
		}

		// EXTERNAL IDENTIFIERS
		static char[] buffer;
		static int bufIndex;
		static State curState;
		static Token curToken;

		public static void Main (string[] args)
		{
			if (args.Length == 0) {
				Console.Write ("No File Found");
				return;
			}
			String tehCode = File.ReadAllText (args.GetValue (0).ToString());
			curToken = getToken (tehCode);
			while(TknType.TKN_EOF != curToken.type){
				if (curToken.type != TknType.TKN_NaN) {
//					Console.WriteLine ("=========================");
					Console.WriteLine ((curToken.type + " --> " + new string (curToken.lexema)));
//					Console.WriteLine ("Current Char" + bufIndex + "of " + buffer.Length);
				}
				curToken = getToken (tehCode);
			}
			Console.WriteLine ((TknType.TKN_EOF + " -->  EOF"));
			return;
		}
	}
}
