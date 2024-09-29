FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-image

ARG FUNCTION_DIR="/build"
ARG SAM_BUILD_MODE="run"
ENV PATH="/root/.dotnet/tools:${PATH}"

RUN apt-get update && apt-get -y install zip

RUN mkdir $FUNCTION_DIR
WORKDIR $FUNCTION_DIR
COPY . $FUNCTION_DIR/
RUN dotnet tool install -g Amazon.Lambda.Tools

# Build and Copy artifacts depending on build mode.
RUN mkdir -p build_artifacts
RUN dotnet lambda package --configuration Release
RUN cp -r /build/bin/Release/net6.0/publish/* /build/build_artifacts

FROM public.ecr.aws/lambda/dotnet:6

COPY --from=build-image /build/build_artifacts/ /var/task/
# Command can be overwritten by providing a different command in the template directly.
CMD ["SignIn::SignIn.Function::FunctionHandler"]
