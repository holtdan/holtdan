/// <reference path="typings/jquery/jquery.d.ts" />

//interface DanAudioMgrOptions {
//    playButtonClass?: string;
//    pauseButtonClass?: string;
//}
interface DanSong {
    $danSong: JQuery;
    $title: JQuery;
    trackNum: number;
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
    private curSongIdx: number;
    constructor(public selector: string) {
        this.$album = $(selector);
        if (this.$album.length == 0)
            throw "No album found.";
        if (this.$album.length > 1)
            throw "Multiple albums found."
        let $songs = this.$album.find(".danSong");
        if ($songs.length == 0)
            throw "No songs found.";

        this.curSongIdx = 0;
        this.songs = $.map($songs, function (item) {
            let $t = $(item);
            let $btn = $t.find(".danPlayPauseButton").first();
            var sng: DanSong = {
                trackNum: parseInt($t.data("tracknum")),
                fileName: $t.data("fname"),
                secsDur: parseInt($t.data("secsdur")),

                $danSong: $t,
                $title: $t.find(".danTitle").first(),
                $button: $btn,
                $btnGlyph: $btn.find("b"),
                $prog: $t.find(".danProgress").first(),
                $bar: $t.find(".danProgressBar").first()
            }
            return sng;
        })
    }
    /**
     * Always a current song!
     */
    public getCurSong(): DanSong {
        let cs = this.songs[this.curSongIdx];
        return cs;
    }
    public setCurSong(idx: number): DanSong {
        this.curSongIdx = idx;
        let cs = this.songs[idx];
        return cs;
    }
    public gotoPrevSong(): DanSong {
        if (--this.curSongIdx < 0)
            this.curSongIdx = this.songs.length - 1;
        let cs = this.songs[this.curSongIdx];
        return cs;
    }
    public gotoNextSong(): DanSong {
        if (++this.curSongIdx > this.songs.length - 1)
            this.curSongIdx = 0;
        let cs = this.songs[this.curSongIdx];
        return cs;
    }
}
class DanAudioMgr {
    private player: any;
    private album: DanSongAlbum;
    private autoPlay: boolean; // enabled by clicking album-level Play button

    constructor(public audioPlayerID: string, public danSongAlbumSelector: string,
        public eventCallback: (triggerElem: JQuery, eventName: string, currentTime?: number) => void) {
        //debugger;
        this.album = new DanSongAlbum(danSongAlbumSelector);
        this.player = document.getElementById(audioPlayerID);
        let self: DanAudioMgr = this;
        self.player.src = "/media/songs/" + self.album.getCurSong().fileName; // prime the pump!

        // browser can start playing
        this.player.oncanplay = function () {
            console.log("oncanplay");
        };
        // media has been started or is no longer paused
        this.player.onplay = function () {
            let sng = self.album.getCurSong();
            //debugger;
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onplay");
            self.toggleBtnPlaying(true);
            sng.$title.addClass("danPlayPauseButtonActive")
            sng.$bar.width(0);
            sng.$prog.addClass("danProgressShow")
        };
        // media is playing after paused or stopped for buffering
        // onplaying -------------------------
        this.player.onplaying = function () {
                    console.log("onplaying");
        };
        // media has paused
        this.player.onpause = function () {
            //debugger;
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onpause");
            self.toggleBtnPlaying(false);
        };
        // current playlist has ended
        this.player.onended = function () {
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "onended");
            //debugger;
            self.setCurrentStopped();
            if (self.autoPlay)
                self.playThis(self.album.gotoNextSong());
            //let $b = $(self.$btnPlaying).find("b");
            //$b.removeClass('danPlayPauseButtonActive')
        };
        // media has advanced
        this.player.ontimeupdate = function () {
            let sng = self.album.getCurSong();
            //debugger;
            var currSecs = parseInt(self.player.currentTime);
            var totSecs = sng.secsDur;
            sng.$bar.css("width", (Math.round((currSecs / totSecs) * 100)) + "%")
            //if (self.eventCallback) self.eventCallback(self.$btnPlaying, "ontimeupdate", secs);
        };
        // browser started looking for media
        this.player.onloadstart = function () {
            console.log("onloadstart");
        }
        $(".danPlayPauseButton").click(function () {
            //debugger;
            if ($(this).hasClass("danPlayPauseAllButton")){
                self.autoPlay = true; // clicked album-level play
                if (self.player.paused)
                    self.player.play();
                else
                    self.player.pause();
            }
            else {
                let $selSng = $(this).closest(".danSong");
                let trackNum = $selSng.data("tracknum");
                let sng = self.album.getCurSong();
                if (sng.trackNum != trackNum) { // clicked play button for different song
                    self.setCurrentStopped();
                    self.playThis(self.album.setCurSong(trackNum));
                }
                else {
                    if (self.player.paused)
                        self.player.play();
                    else
                        self.player.pause();
                }
            }
        })
    }
    private playThis(sng: DanSong) {
        let playName = "/media/songs/" + sng.fileName;
        this.player.src = playName;
        this.player.play();
    }
    private setCurrentStopped() {
        let cs: DanSong = this.album.getCurSong();

        cs.$title.removeClass("danPlayPauseButtonActive");
        cs.$button.removeClass("danPlayPauseButtonActive");
        cs.$btnGlyph.removeClass("glyphicon-pause");
        cs.$btnGlyph.addClass("glyphicon-play");
        cs.$prog.removeClass("danProgressShow");
    }
    private toggleBtnPlaying(on: boolean): void {
        let $b = this.album.getCurSong().$button.find(".glyphicon").first();
        let $masterB = this.album.$album.find(".glyphicon").first();
        if (on) {
            $masterB.removeClass("glyphicon-play");
            $masterB.addClass("glyphicon-pause");
            $b.removeClass("glyphicon-play");
            $b.addClass("glyphicon-pause");
        }
        else {
            $masterB.removeClass("glyphicon-pause");
            $masterB.addClass("glyphicon-play");
            $b.removeClass("glyphicon-pause");
            $b.addClass("glyphicon-play");
        }
    }
}
