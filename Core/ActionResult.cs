namespace Rogue.Actions {
    public enum Outcome {
        Success,
        Failure,
        Canceled
    }

    public class ActionResult {
        public ActionResult(Outcome outcome, string message, bool keepMoving) {
            Message = message;
            Outcome = outcome;
            KeepMoving = keepMoving;
        }

        public string Message { get; }
        public Outcome Outcome { get; }
        public bool KeepMoving { get; }


        public static ActionResult Succeed (string message, bool keepMoving) {
            return new ActionResult(Outcome.Success, message, keepMoving);
        }

        public static ActionResult Fail(string message, bool keepMoving) {
            return new ActionResult(Outcome.Failure, message, keepMoving);
        }

        public static ActionResult Cancel(string message, bool keepMoving) {
            return new ActionResult(Outcome.Canceled, message, keepMoving);
        }
    }
}
