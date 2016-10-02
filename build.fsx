// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake

// Properties
let buildDir = "./build/"
let testDir  = "./test/"
let nugetConfig = "./nuget.config"
let projects = !! "src/**/project.json"
let dotnetPath = "./.dotnet/dotnet.exe"

// Commands
let dotnetRestore path = ignore(Shell.Exec(dotnetPath, "restore " + path))
let dotnetBuild path = ignore(Shell.Exec(dotnetPath, "build " + path))

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir]
)

Target "Build" (fun _ ->
    trace "Building projects:"
    projects
        |> Seq.iter(fun proj ->
            dotnetRestore proj
            dotnetBuild proj
        )
)

// Start build
RunTargetOrDefault "Build"
