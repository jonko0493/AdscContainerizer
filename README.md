# ADSC Containerizer

Just a quick tool to be able to create ADSC containerized ADS sound files for PSX/PS2 games.

Relies on [psxavenc](https://github.com/WonderfulToolchain/psxavenc) for audio encoding.

## Usage
```
AdscContainerizer -s [path/to/audio/file] -e [/path/to/psxavenc] -o [/path/to/output/file] -f [audio sample rate] -i [interleave amount]
```

Note that only stereo audio is supported.

Please ensure that the audio sample rate you specify matches the output from `ffprobe` (or similar). If you need to adjust the audio sample rate before using this tool, use `ffmpeg` to do so.