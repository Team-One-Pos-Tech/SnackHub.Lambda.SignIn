variable "LabRoleName" {
  description = "Name for the LabRole IAM role"
  default     = "LabRole"
}

variable "functionName" {
  description = "Name for the Lambda function"
  default     = "signin_lambda"
}

variable "clientId" {
  description = "Client ID for the Cognito User Pool"
  default = "4g9i9qigcm7mq82s2r7v939uae"
}

variable "userPollId" {
  description = "Client Secret for the Cognito User Pool"
  default = "us-east-1_DBk6tjf8T"
}

variable "regionDefault" {
  description = "Default region for the AWS provider"
  default     = "us-east-1"
}