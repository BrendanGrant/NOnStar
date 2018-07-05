using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.Types
{
    class KnownCommand
    {
        public static KnownCommand CancelAlert = new KnownCommand("cancelAlert", "Cancel a vehicle alert(honk horns/flash lights).");
        public static KnownCommand LockDoor = new KnownCommand("lockDoor", "Locks the doors.");
        public static KnownCommand UnlockDoor = new KnownCommand("unlockDoor", "Unlocks the doors.");
        public static KnownCommand Alert = new KnownCommand("alert", "Triggers a vehicle alert (honk horns/flash lights).");
        public static KnownCommand Start = new KnownCommand("start", "Remotely starts the vehicle.");
        public static KnownCommand CancelStart = new KnownCommand("cancelStart", "Cancels previous remote start command.");
        public static KnownCommand SendNavDestination = new KnownCommand("sendNavDestination", "Calculate route and send it to the vehicle's nav unit.");
        public static KnownCommand Connect = new KnownCommand("connect", "Initiates a connection to the vehicle");
        public static KnownCommand EnableTelemetry = new KnownCommand("enableTelemetry", "Enrolls a vehicle in telemetry.");
        public static KnownCommand DisableTelemetry = new KnownCommand("disableTelemetry", "Unenrolls a vehicle from telemetry..");

        public string Key { get; set; }
        public string Description { get; set; }

        private KnownCommand(string key, string description)
        {
            this.Key = key;
            this.Description = description;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator==(string key, KnownCommand command)
        {
            return key == command.Key;
        }

        public static bool operator !=(string key, KnownCommand command)
        {
            return key != command.Key;
        }

        public static bool operator ==(KnownCommand command, string key)
        {
            return key == command.Key;
        }

        public static bool operator !=(KnownCommand command, string key)
        {
            return key != command.Key;
        }
    }
}
