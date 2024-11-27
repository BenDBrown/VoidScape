# Context
Here you want to write why this pull request was made. Information about your push. 
Giving context is important to allow for a better understanding on why it has been merged in.\
For Example:\
Ship can now be built using a UI instead of the old builder. Through these changes I had to create a new system that allows icons to be drag and dropped.
# Additions
Here you want to specify what has been added Feature wise. For Example:
* A UI System that drags and drops
* A settings menu to allow faster changing of stuff
# Modified
Here are files or features that had to be modified due to this request. For Example:
* Ship.cs
* PlayerShip Prefab to add a icon control node
# Removed
Specify here files that were removed and for what reason. These do not need to be detailed and not every file but only those deemed important.
* Removed Ship.cs due to redundancy
* Removed Draggable.cs because it's not used anymore
# Asset Change
This is required if an asset(art) has been changed. An asset like a sprite sheet or an aseprite file will cause conflict so add it here.
# Bug
Any bugs introduced but left in because they are considered minor/not game breaking. This should only be added due to time constraints or if trying to prototype.
* Bug when trying to load ship after saving twice.
# Bug Fix
What bugs were fixed in this Pull Request. Use the issue board number if it exists so we can see how to test if it was fixed.
* Fixes #10
* Closes VoidScape/double-save#10
