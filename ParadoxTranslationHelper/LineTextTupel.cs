﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper
{
    public class LineTextTupel
    {
        public int LineNumber;
        public string Token;
        public string LineText;

        public LineTextTupel( int lineNumber, string token )
        {
            LineNumber = lineNumber;
            Token = token;
        }

        public override bool Equals(object obj) => this.Equals(obj as LineTextTupel);

        public bool Equals(LineTextTupel lineTextTupel)
        {
            if(lineTextTupel is null )
            {
                return false;
            }

            if (Object.ReferenceEquals(this, lineTextTupel))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != lineTextTupel.GetType())
            {
                return false;
            }

            return lineTextTupel.Equals(this.Token);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LineNumber, Token);
        }
    }
}
