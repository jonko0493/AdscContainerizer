# ADSC Containerizer

Just a quick tool to be able to create ADSC containerized ADS sound files for PSX/PS2 games.

Relies on [psxavenc](https://github.com/WonderfulToolchain/psxavenc) for audio encoding.

## Usage
```
AdscContainerizer -s [path/to/audio/file] -e [/path/to/psxavenc] -o [/path/to/output/file] -f [audio sample rate] -c [number of audio channels] -i [interleave amount]
```