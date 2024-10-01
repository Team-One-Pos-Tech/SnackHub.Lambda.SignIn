resource "aws_apigatewayv2_api" "signin_lambda_api_gateway" {
  name          = "signin_lambda_http_api"
  protocol_type = "HTTP"
}

resource "aws_apigatewayv2_stage" "signin_lambda_api_gateway_stage" {
  api_id      = aws_apigatewayv2_api.signin_lambda_api_gateway.id
  name        = "Prod"
  auto_deploy = true
}

resource "aws_apigatewayv2_integration" "signin_lambda_api_gateway_integration" {
  api_id                 = aws_apigatewayv2_api.signin_lambda_api_gateway.id
  integration_type       = "AWS_PROXY"
  integration_method     = "POST"
  integration_uri        = aws_lambda_function.signin_lambda.invoke_arn
  payload_format_version = "2.0"
}

resource "aws_apigatewayv2_route" "signin_lambda_api_gateway_route" {
  api_id    = aws_apigatewayv2_api.signin_lambda_api_gateway.id
  route_key = "POST /signIn"
  target    = "integrations/${aws_apigatewayv2_integration.signin_lambda_api_gateway_integration.id}"
}

resource "aws_lambda_permission" "aws_lambda_api_gateway_permission" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.signin_lambda.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_apigatewayv2_api.signin_lambda_api_gateway.execution_arn}/*/POST/signIn"
}

output "aws_apigatewayv2_url" {
  value = aws_apigatewayv2_api.signin_lambda_api_gateway.api_endpoint
}