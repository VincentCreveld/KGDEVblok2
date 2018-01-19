using System;

public static class ExtensionFunctions {

    public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget) {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static int Map(int value, int fromSource, int toSource, int fromTarget, int toTarget) {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static double Map(double value, double fromSource, double toSource, double fromTarget, double toTarget) {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static decimal Map(this decimal value, decimal fromSource, decimal toSource, decimal fromTarget, decimal toTarget) {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }


    public static float RoundToMultiple(float value, float multiple) {
        return (float)Math.Round(value / multiple) * multiple;
    }

    public static double RoundToMultiple(double value, double multiple) {
        return Math.Round(value / multiple) * multiple;
    }

    public static decimal RoundToMultiple(decimal value, decimal multiple) {
        return Math.Round(value / multiple) * multiple;
    }

}
