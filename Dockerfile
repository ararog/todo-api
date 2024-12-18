# syntax=docker/dockerfile:1
ARG DOTNET_VERSION=9.0
ARG CODENAME=alpine

ARG SOURCE_DIR=/home/jenkins

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-${CODENAME} AS base

FROM base AS builder

ARG SOURCE_DIR

WORKDIR "$SOURCE_DIR"

COPY --link *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish --no-restore -o /app

FROM builder AS test

ARG SOURCE_DIR

WORKDIR "$SOURCE_DIR"

RUN pnpm run test

FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime

ARG SOURCE_DIR

WORKDIR /app
COPY --link --from=builder /app .
USER $APP_UID

#ENTRYPOINT ["./bin/Release/net9.0/publish/TodoApi"]
ENTRYPOINT ["tail", "-f", "/dev/null"]
