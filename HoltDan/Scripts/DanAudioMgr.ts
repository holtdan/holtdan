/// <reference path="typings/jquery/jquery.d.ts" />

interface DanAudioMgrOptions {
    playButtonClass?: string;
    pauseButtonClass?: string;
}
interface DanSong {
    $danSong: JQuery;
    $title: JQuery;
    fileName: string;
    secsDur: number;
    $button: JQuery;
    $btnGlyph: JQuery;
    $prog: JQuery;
    $bar: JQuery;
}
class DanSongAlbum { // should derive from DanAlbum so can DanPhotoAlbum, etc.
    $album: JQuery;
    songs: Array<DanSong>;
    curSong: DanSong;
    constructor(public selector: string) {
        this.$album = $(selector);
        if (this.$album.length == 0)
            throw "No album found.";
        if (this.$album.length > 1)
            throw "Multiple albums found."
        let $songs = this.$album.find(".danSong");
        if ($songs.length == 0)
            throw "No songs found.";

        this.songs = $.map($songs, function (item) {
            let $t = $(item);
            let $btn = $t.find(".danPlayPauseButton").first();
            var sng: DanSong = {
                $danSong: $t,
                $title: $t.find(".danTitle").first(),
                $button: $btn,
                $btnGlyph: $btn.find("b"),
                fileName: $btn.data("fname"),
                secsDur: parseInt($btn.data("secsdur")),
                $prog: $t.find(".danProgress").first(),
                $bar: $t.find(".danProgressBar").first()
            }
            return sng;
        })
    }
    private ButtonIsCurrent(button: JQuery): boolean {
        if (!this.curSong) return false;

        let inFN = button.data("fname")
        return this.curSong.fileName == inFN;
    }
    public GetSongFromButton(button: JQuery): DanSong {
        let self = this;
        let inFN = button.data("fname")
        let one = jQuery.grep(self.songs, function (item, idx) {
            let itmFN = item.$button.data("fname")
            return itmFN == inFN;
        });
        return one[0];
    }
}
class DanAudioMgr {
    private player: any;
    private album: DanSongAlbum;

    constructor(public audioPlayerID: string, public danSongAlbumSelector: string,
        public eventCallback: (triggerElem: JQuery, eventName: string, currentTime?: number) => void) {

        this.album = new DanSongAlbum(danSongAlbumSelector);
        this.player = document.getElementById(audioPlayerID);
        let self: DanAudioMgr = this;

        this.player.onplay = function () {
            let sng = self.album.curSong;
            //debugger;
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onplay");
            self.toggleBtnPlaying(true);
            sng.$title.addClass("danPlayPauseButtonActive")
            sng.$bar.width(0);
            sng.$prog.addClass("danProgressShow")
        };
        this.player.onpause = function () {
            //debugger;
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onpause");
            self.toggleBtnPlaying(false);
        };
        this.player.onended = function () {
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onended");
            self.reset();
            //let $b = $(self.$btnPlaying).find("b");
            //$b.removeClass('danPlayPauseButtonActive')
        };
        this.player.ontimeupdate = function () {
            let sng = self.album.curSong;
            //debugger;
            var currSecs = parseInt(self.player.currentTime);
            var totSecs = sng.secsDur;
            sng.$bar.css("width", (Math.round((currSecs / totSecs) * 100)) + "%")
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "ontimeupdate", secs);
        };
        this.player.onloadstart = function () {
            debugger;
        }
        this.player.oncanplay = function () {
            debugger;
        }
        $(".danPlayPauseButton").click(function () {
            let sng = self.album.GetSongFromButton($(this));
            if (!self.album.curSong || self.album.curSong.fileName != sng.fileName) {
                self.reset();
                let playName = "/media/songs/" + sng.fileName;
                self.album.curSong = sng;
                self.player.src = playName;
                self.player.play();
            }
            else {
                if (self.player.paused)
                    self.player.play();
                else
                    self.player.pause();
            }
        })
    }
    private reset() {

        if (this.album.curSong) {
            let cs: DanSong = this.album.curSong;

            cs.$title.removeClass("danPlayPauseButtonActive");
            cs.$button.removeClass("danPlayPauseButtonActive");
            cs.$btnGlyph.removeClass("glyphicon-pause");
            cs.$btnGlyph.addClass("glyphicon-play");
            cs.$prog.removeClass("danProgressShow");

            this.album.curSong = null;
        }
    }
    private toggleBtnPlaying(on: boolean): void {
        let $b = this.album.curSong.$button.find(".glyphicon").first();
        if (on) {
            $b.removeClass("glyphicon-play");
            $b.addClass("glyphicon-pause");
        }
        else {
            $b.removeClass("glyphicon-pause");
            $b.addClass("glyphicon-play");
        }
    }
}