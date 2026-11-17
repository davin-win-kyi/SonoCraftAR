# SonoCraftAR

SonoCraftAR is a Unity research prototype for authoring sound-reactive visual interfaces. A user describes a visualization in natural language, the application generates Unity C# code for it, and Roslyn compiles and loads that code at runtime. The generated UI exposes a method that receives sound measurements, so its appearance can change as those values change.

The project explores how visual feedback can make sound easier to understand, including for deaf and hard-of-hearing users. For example, a user could request a wave whose amplitude changes with sound volume.

## How it works

1. **Describe a visualization.** The user enters a request in the Unity UI. `PromptEnhancement` sends it to the language model and prepares a more detailed design prompt.
2. **Generate C# UI code.** `ScriptGeneration` combines the request, design guidance, and any saved prior prompt/script with the generation prompt. The model's response is checked for C# diagnostics and passed through a correction step. The resulting script and prompt are saved locally as `Assets/SCRIPTS/Application/Json/Scripts.json`.
3. **Compile and load in Unity.** `ScriptsRun` listens for the script-generated event, creates a Roslyn `ScriptDomain`, and calls `CompileAndLoadMainSource` on the generated source. It creates a Unity `GameObject` for the resulting script instance, allowing the new visualization to appear without manually adding a compiled script to the project.
4. **Send sound data to the UI.** The generated code is expected to provide a `GeneratedFunction(object[] soundData)` entry point. The current runner packages each sample as an array of `volume`, `frequency`, and `pitch`, then calls the generated method repeatedly (the default update interval is 0.01 seconds). Generated code can use those values to change UI properties such as size, position, color, or animation.
5. **Render the response.** The generated visualization uses Unity and the project's Shapes library to draw and animate its UI. This closes the loop from a natural-language request to a sound-responsive visual.

## Sound input status

The current sound-data injection path is a **mock-data prototype**. `MockUtils.USE_MOCK_SOUND_DATA` is enabled by default, and `ScriptsRun` reads a sample JSON dataset (currently `MeowingSoundData.json`) and cycles through its `SoundData` entries. The sound-data model contains `volume`, `frequency`, and `pitch`.

There is also a `RecordingAudio` component that records microphone audio and saves a trimmed WAV file. At present, that recording is **not** connected to a feature-extraction step or to `ScriptsRun`'s sound-data injection loop. Consequently, the runtime visualization updates in the current flow are driven by the mock samples, not by live microphone measurements. Connecting captured audio to measured sound features and then to the generated UI is future integration work.

## Requirements

- Unity `2022.3.52f1` (see `ProjectSettings/ProjectVersion.txt`)
- An OpenAI API key for prompt enhancement, code generation, and the script-correction request
- The Roslyn C# scripting assets and Shapes library included in this repository

## Getting started

1. Open the repository in the specified Unity version.
2. In the repository root, copy `.env.example` to `.env` and set `openAIKey` to your API key. Keep `.env` private; it is ignored by Git.
3. Open the project's main scene and run it in the Unity Editor.
4. Enter a visualization request in the prompt UI and use the UI's generation action. The generated script is saved under `Assets/SCRIPTS/Application/Json/` and compiled/loaded by `ScriptsRun`.

The repository is a prototype, so the exact scene controls and generated-script behavior may evolve. The runtime generation path requires the language-model service to be reachable and a valid key to be configured.

## Project layout

- `Assets/SCRIPTS/Application/` — prompt enhancement, C# generation/checking, Roslyn runtime integration, sound utilities, and JSON data
- `Assets/SCRIPTS/Application/Json/` — generated script state and sample sound-data datasets
- `Assets/SCRIPTS/Prototypes/` — UI and runtime experiments
- `Assets/RoslynCSharp/` — Roslyn C# runtime/compiler integration
- `Assets/Shapes/` — Shapes drawing library used by visualizations
- `Assets/Scenes/` — Unity scenes
- `ProjectSettings/` and `Packages/` — Unity project configuration and package manifest

## Security note

Generated C# is compiled and executed inside the Unity application. Treat generated code as executable code: review prompts and generated behavior, and do not use untrusted scripts. The API key belongs only in the ignored local `.env` file; never commit it or serialize it into a Unity scene or asset.
