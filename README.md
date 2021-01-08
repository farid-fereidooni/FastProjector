# FastProjector

## Under Production

Fast projector is a lightweight project for projecting lists into other model, which is mainly used in Database querying.

Unlike similar project, Fast projector is not going to use reflection or at least as small as possible for sake of applications' first bootstap.

The term 'Fast' is used because Fast Projectors is going to use the new Roslyn feature, Source Generator, for creating and caching all projection functions and expressions at compile time so that there will be a huge boost for applications with lots of model projection.

The project is at early production stage, and soon there will a release with small portion of features for experiment.
