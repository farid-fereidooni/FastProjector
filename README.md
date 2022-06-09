# FastProjector

![Build and Test Workflow](https://github.com/farid-fereidooni/FastProjector/actions/workflows/dotnet.yml/badge.svg)

## Under Production

Fast projector is a lightweight library for projecting lists into other models, which is mainly used in Database querying.

Unlike similar project, Fast projector is not going to use reflection or at least as small as possible for sake of applications' first bootstap.

The term 'Fast' is used because Fast Projector is going to use the new Roslyn feature; "Source Generator"; for creating and caching all projection functions and expressions at compile time so that there will be a huge boost for applications with lots of model projection.

This project is at it's early production stage and soon there will a release with small portion of features for experiment.
