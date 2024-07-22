Metadata Editor
===============
This is a CLI for managing metadata for digital music files. I had recently re-ripped my entire CD collection to lossless files and to higher-quality MP3 files. As
part of that I wanted to enforce some naming rules for artists, songs, and albums. I tried to make sure all new files followed them but with almost 7,000 tracks, 
some were bound to get skipped. Hence this utility to find any issues so I can correct them. I wanted the new collection to be as consistent as possible.

It can perform various operations on a directory of music files, such as:

- Check casing of file names and metadata attributes.
- Search for specific text in a file name or metadata attribute. This is for finding things where I changed my mind about the naming and I wanted to find occurrences of the old naming.
- Search for files with a specific metadata attribute that is empty or non-empty.
- Search for file names or metadata attrbutes that have an open parentheses followed by a lower-case letter.

Currently it only checks the artist, title, album, and album artist metadata values. You pass it the extension of the files to check, such as 'flac' or 'mp3', 
and the path to the output file that contains the results of the operation. That file will include the full path of all files that need attention, plus the 
erroneous metadata value if applicable.
