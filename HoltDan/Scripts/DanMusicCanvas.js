var MusicCanvas = (function () {
    function MusicCanvas() {
    }
    MusicCanvas.DrawGuitarNeck = function (opts) {
        if (!opts.numStrings)
            opts.numStrings = 6;
        if (!opts.numFrets)
            opts.numFrets = 22;
        if (!opts.tuning)
            opts.tuning = ["E", "A", "D", "G", "B", "E"];
    };
    return MusicCanvas;
}());
//# sourceMappingURL=DanMusicCanvas.js.map