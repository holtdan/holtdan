interface ScalesOptions {
    visitID: number;
    urlGetContent: string;
    modalTitle: string;
    closeButtonText: string;
}
class Scales {
    public notes = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"];
    private steps = [2, 2, 1, 2, 2, 2, 1];
    private pentIdxs = [0, 2, 3, 4, 6];
    private scaleChordForms = ["I", "ii", "iii", "IV", "V", "vi", "viio"];
    private numOcts = 2;
    private keyIdx = 7;
    private mode = 0;

    constructor() {
        let self: Scales = this;
        $("#pickKey").click(function () {
            $("#selKey").removeClass('danSlideOut')
            $("#selKey").addClass('danSlideIn')
        })
        $(".optSelect").click(function (elem) {
            self.HideOptDivs();
            let offset = $(this).attr("data-key");
            self.keyIdx = parseInt(offset);
            self.setScale();
            self.getNotes();
        });
        $(".step").click(function () {
            self.HideOptDivs();
        });
        window.addEventListener('resize', function () {
            self.scaleScale();
        })
        this.setScale();
    }
    private totalSteps() : number {
        return this.numOcts * this.steps.length + 1; // +1 = add root at end
    }
    private getNoteString(useNotes: Array<number>, useIdx: number): string {
        let shiftIdx = useIdx % useNotes.length;
        return this.notes[useNotes[shiftIdx]];
    }
    private getNotes() : Array<number> {
        let useNotes = new Array();
        let curStep = this.mode;
        let atIdx = this.keyIdx;
        for (var i = 0; i < this.steps.length; i++) {
            let useIdx = atIdx % this.notes.length
            useNotes.push(useIdx);

            atIdx += this.steps[curStep];

            if (++curStep > this.steps.length - 1)
                curStep = this.mode;
        }
        //debugger;
        return useNotes;
    }
    private HideOptDivs() {
        $("#selKey").removeClass('danSlideIn')
        $("#selKey").addClass('danSlideOut')
    }
    private scaleScale() : void {
        let cx = window.innerWidth;
        let cy = window.innerHeight;
        //$("#debug").text(cx + ',' + cy);
        let cyPer = cy / this.totalSteps();
        $(".step").css('height', cyPer + "px")
        $(".noteName").css('line-height', cyPer + "px")
        $(".noteName").css('font-size', Math.floor(cyPer * .66) + "px")
    }
    private setScale() {
        let useNotes = this.getNotes();
        let unLen = useNotes.length;
        for (var i = 0; i < this.totalSteps(); i++) {
            let useIdx = useNotes[i % unLen];
            //let root = this.getNoteString()
            let $step = $("#step" + i)

            //$step.find(".noteName").text (this.getNoteString(useNotes, i % useNotes.length));
            $step.find(".noteName") .text(this.getNoteString(useNotes, i));
            $step.find(".chordForm").text(this.scaleChordForms[i % this.scaleChordForms.length]);
            $step.find(".root").text(this.getNoteString(useNotes, i));
            $step.find(".third").text(this.getNoteString(useNotes, i + 2));
            $step.find(".fifth").text(this.getNoteString(useNotes, i + 4));
            $step.find(".seventh").text(this.getNoteString(useNotes, i + 6));
        }
        this.scaleScale();
    }
}
