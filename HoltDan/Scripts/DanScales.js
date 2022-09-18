var Scales = /** @class */ (function () {
    function Scales() {
        this.notes = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"];
        this.steps = [2, 2, 1, 2, 2, 2, 1];
        this.pentIdxs = [0, 2, 3, 4, 6];
        this.scaleChordForms = ["I", "ii", "iii", "IV", "V", "vi", "viio"];
        this.modes = ["Ionian", "Dorian", "Phrygian", "Lydian", "Mixolydian", "Aeolian", "Locrian"];
        this.numOcts = 2;
        this.keyIdx = 7;
        this.mode = 0;
        var self = this;
        $("#pickKey").click(function () {
            $("#selKey").removeClass('danSlideOut');
            $("#selKey").addClass('danSlideIn');
        });
        $(".optSelect").click(function (elem) {
            self.HideOptDivs();
            var offset = $(this).attr("data-key");
            self.keyIdx = parseInt(offset);
            self.setScale();
            self.getNotes();
        });
        $(".step").click(function () {
            self.HideOptDivs();
        });
        window.addEventListener('resize', function () {
            self.scaleScale();
        });
        this.setScale();
    }
    Scales.prototype.totalSteps = function () {
        return this.numOcts * this.steps.length + 1; // +1 = add root at end
    };
    Scales.prototype.getNoteString = function (useNotes, useIdx) {
        var shiftIdx = useIdx % useNotes.length;
        return this.notes[useNotes[shiftIdx]];
    };
    Scales.prototype.getNotes = function () {
        var useNotes = new Array();
        var curStep = this.mode;
        var atIdx = this.keyIdx;
        for (var i = 0; i < this.steps.length; i++) {
            var useIdx = atIdx % this.notes.length;
            useNotes.push(useIdx);
            atIdx += this.steps[curStep];
            if (++curStep > this.steps.length - 1)
                curStep = this.mode;
        }
        //debugger;
        return useNotes;
    };
    Scales.prototype.HideOptDivs = function () {
        $("#selKey").removeClass('danSlideIn');
        $("#selKey").addClass('danSlideOut');
    };
    Scales.prototype.scaleScale = function () {
        var cx = window.innerWidth;
        var cy = window.innerHeight;
        //$("#debug").text(cx + ',' + cy);
        var cyPer = cy / this.totalSteps();
        $(".step").css('height', cyPer + "px");
        $(".noteName").css('line-height', cyPer + "px");
        $(".noteName").css('font-size', Math.floor(cyPer * .66) + "px");
    };
    Scales.prototype.setScale = function () {
        var useNotes = this.getNotes();
        var unLen = useNotes.length;
        for (var i = 0; i < this.totalSteps(); i++) {
            var useIdx = useNotes[i % unLen];
            //let root = this.getNoteString()
            var $step = $("#step" + i);
            //$step.find(".noteName").text (this.getNoteString(useNotes, i % useNotes.length));
            $step.find(".noteName").text(this.getNoteString(useNotes, i));
            $step.find(".chordForm").text(this.scaleChordForms[i % this.scaleChordForms.length]);
            $step.find(".root").text(this.getNoteString(useNotes, i));
            $step.find(".third").text(this.getNoteString(useNotes, i + 2));
            $step.find(".fifth").text(this.getNoteString(useNotes, i + 4));
            $step.find(".seventh").text(this.getNoteString(useNotes, i + 6));
        }
        this.scaleScale();
    };
    return Scales;
}());
//# sourceMappingURL=DanScales.js.map