var Scales = (function () {
    function Scales() {
        this.notes = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"];
        this.steps = [2, 2, 1, 2, 2, 2, 1];
        this.pentIdxs = [0, 2, 3, 4, 6];
        this.scaleChordForms = ["I", "ii", "iii", "IV", "V", "vi", "viio"];
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
        return this.notes[useNotes[useIdx]];
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
        for (var i = 0; i < this.totalSteps(); i++) {
            $("#step" + i).find(".noteName").text(this.getNoteString(useNotes, i % useNotes.length));
            $("#step" + i).find(".interval0").text(this.getNoteString(useNotes, i % useNotes.length));
            $("#step" + i).find(".interval2").text(this.getNoteString(useNotes, (i + 2) % useNotes.length));
            $("#step" + i).find(".interval4").text(this.getNoteString(useNotes, (i + 4) % useNotes.length));
            $("#step" + i).find(".interval6").text(this.getNoteString(useNotes, (i + 6) % useNotes.length));
            $("#step" + i).find(".chordForm").text(this.scaleChordForms[i % this.scaleChordForms.length]);
        }
        this.scaleScale();
    };
    return Scales;
}());
//# sourceMappingURL=DanScales.js.map