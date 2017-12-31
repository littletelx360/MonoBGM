@Echo off

nuget.exe pack MonoBGM\MonoBGM.csproj -properties Configuration=Release -properties Platform=AnyCPU
nuget.exe pack MonoBGM.ContentPipeline\MonoBGM.ContentPipeline.csproj -properties Configuration=Release -properties Platform=AnyCPU
