Metadata Editor
===============
This is a CLI for managing metadata for digital music files. I had recently re-ripped my entire CD collection to lossless files and to higher-quality MP3 files. As
part of that I wanted to enforce some naming rules for artists, songs, and albums. I tried to make sure all new files followed them but with 6,500+ 
tracks, some were bound to get skipped. Hence this utility. I wanted the collection to be as consistent as possible.

It can perform various operations on a directory of music files, such as:

- Check casing of file names
- Check casing of metadata attributes. Currently it only checks artists, title, and album.
- Search for specific text in a metadata attribute. This is for finding things where I changed my mind about the naming and I wanted to find occurrences of the old naming.

You pass it the extension of the files to check, such as 'flac' or 'mp3', and the path to the output file that contains the results of the operation. That file will 
include the full path of all files that need attention, plus the erroneous metadata value if applicable.
