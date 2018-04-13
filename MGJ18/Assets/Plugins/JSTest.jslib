// https://docs.unity3d.com/560/Documentation/Manual/webgl-interactingwithbrowserscripting.html
mergeInto(LibraryManager.library, {
  CheckRecord: function () {
    checkRecord();
  },
  SetBackgroundColor: function (color) {
    setBackgroundColor(Pointer_stringify(color));
  },
  StoreEmotion: function (id) {
    storeEmotion(id);
  },
  KillMe: function() {
    killGame();
  }
});