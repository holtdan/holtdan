interface Size {
    width: number;
    height: number;
}
interface GuitarNeckOptions extends Size {
    numStrings?: number;
    numFrets?: number;
    tuning?: Array<string>;
}
class MusicCanvas {
    public static DrawGuitarNeck(opts: GuitarNeckOptions): void {
        if (!opts.numStrings) opts.numStrings = 6;
        if (!opts.numFrets) opts.numFrets = 22;
        if (!opts.tuning) opts.tuning = ["E","A","D","G","B","E"];
    }
}
