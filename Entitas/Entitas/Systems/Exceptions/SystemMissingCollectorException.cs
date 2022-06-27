using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitas {
    public class SystemMissingCollectorException : EntitasException {
        public SystemMissingCollectorException(string message, string hint)
            : base(message + $"\nReactiveSystems cannot have a null ICollector!", hint) {
        }
    }
}
