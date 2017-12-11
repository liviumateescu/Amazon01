using System;
using System.Collections.Generic;
using System.Text;

namespace Cheop.Exceptions
{
    class RepetaAutostradaException : Exception
    {
        public RepetaAutostradaException(string message):base(message)
        {
        }
        public RepetaAutostradaException(string message, Exception ex) : base(message, ex)
        {
        }
    }

    class PreaMulteOraseException : Exception
    {
        public PreaMulteOraseException(string message) : base(message)
        {
        }
        public PreaMulteOraseException(string message, Exception ex) : base(message, ex)
        {
        }
    }

    class PreaMulteAutostraziException : Exception
    {
        public PreaMulteAutostraziException(string message) : base(message)
        {
        }
        public PreaMulteAutostraziException(string message, Exception ex) : base(message, ex)
        {
        }
    }

    class FisierInconsistentException : Exception
    {
        public FisierInconsistentException(string message) : base(message)
        {
        }
        public FisierInconsistentException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
