data "aws_caller_identity" "current" {}

locals {
  account_id = data.aws_caller_identity.current.account_id
  LabRoleArn = "arn:aws:iam::${local.account_id}:role/${var.LabRoleName}"
}

resource "aws_lambda_function" "signin_lambda" {
  function_name = var.functionName
  role          = local.LabRoleArn
  image_uri     = "986099789222.dkr.ecr.us-east-1.amazonaws.com/signin-lambda:latest"
  package_type  = "Image"
  timeout       = 15

  environment {
    variables = {
      CLIENT_ID     =  var.clientId
      USER_POOL_ID  =  var.userPollId
    }
  }
}
