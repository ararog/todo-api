# syntax=docker/dockerfile:1
ARG DOTNET_VERSION=9.0
ARG CODENAME=alpine

ARG SOURCE_DIR=/home/jenkins

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-${CODENAME} AS base

FROM base AS builder

ARG TARGETARCH
ARG SOURCE_DIR

WORKDIR "$SOURCE_DIR"

COPY --link *.csproj .
RUN dotnet restore -a $TARGETARCH

COPY . .
RUN dotnet publish -a $TARGETARCH --no-restore -o /app

FROM builder AS test

ARG SOURCE_DIR

WORKDIR "$SOURCE_DIR"

RUN pnpm run test

FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine as runtime

ARG SOURCE_DIR

WORKDIR /app
COPY --link --from=build /app .
USER $APP_UID

ENTRYPOINT ["./dotnetapp"]
