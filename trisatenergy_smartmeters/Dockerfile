FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /app

ARG REPO_SOURCE=https://github.com/TriSAT-Collective/smart-meter-simulation
ARG DESCRIPTION="simulates smart meter data"
ARG ARTIFACT_PATH=dist/

ARG ENTRYPOINT=/app/trisatenergy_smartmeters
ENV ENTRYPOINT=$ENTRYPOINT

LABEL org.opencontainers.image.source=$REPO_SOURCE
LABEL org.opencontainers.image.description=$DESCRIPTION

COPY $ARTIFACT_PATH /app

ENTRYPOINT $ENTRYPOINT
CMD []
