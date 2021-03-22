# Save-Update
 UI for administration of backups on multiple drives

Save: “Source”, “Target” and dependent Text Boxes are dynamically generated according to the setting of the “Sources” Combo Box (1-12 possible sources). Files/directories according to the paths in each source Text Box will be copied to the corresponding paths in “Target” boxes for all the drives from the corresponding “Drives” box using RoboCopy.exe. Saving/updating has two modes: 

- “single target mode”, when data from each source will be copied to the individual target destination on all the indicated drives
- “multiple target mode”, data from a single source are copied to multiple target destinations

Path projects can be saved and edited. Path projects (source paths, target paths, drives and excluded drives) will be mamaged through the “Project” ComboBox and buttons:

Change: saves a changed path project.
New: creates a new path project.
Delete: deletes a path project.

Interface also takes into account a possible “Home” directory for placing data, that might be present on a drive.

Additionally interface allows to delete files/directories from all indicated drives.