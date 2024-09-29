FROM public.ecr.aws/lambda/dotnet:6
  
COPY . ${LAMBDA_TASK_ROOT}
  
CMD [ "SignIn::SignIn.Function::FunctionHandler" ]