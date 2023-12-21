UI Guide List - make a list of guides explored by players

All in-game guides need to be declared in scriptable object "GuideList"

Whenever player activates an event ref to a guide, call function "UnlockGuide(id)" from DataManager to add guide
to unlocked list