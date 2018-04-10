// https://docs.unity3d.com/560/Documentation/Manual/webgl-interactingwithbrowserscripting.html
mergeInto(LibraryManager.library, {
  PutInList: function (id, color) {
    putInList(id, Pointer_stringify(color));
  },
  CheckRecord: function () {
    checkRecord();
  },
  SetBackgroundColor: function (color) {
    setBackgroundColor(Pointer_stringify(color));
  }
});