pipeline {
  agent any
  stages {
    stage('error') {
      steps {
        dotnetClean(outputDirectory: 'publish', project: 'Botticelli', showSdkInfo: true, workDirectory: 'Botticelli', charset: 'Utf8', framework: 'net7.0', nologo: true)
      }
    }

  }
}