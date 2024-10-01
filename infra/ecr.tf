resource "aws_ecr_repository" "sign_in_auth" {
    name                 = "sign-in-registry"
    image_tag_mutability = "MUTABLE"
    image_scanning_configuration {
        scan_on_push = true
    }
}