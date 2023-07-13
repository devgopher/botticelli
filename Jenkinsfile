pipeline {
  agent any
  stages {
    stage('back_publish_dev') {
      steps {
        dotnetClean(outputDirectory: 'publish', project: 'Botticelli', showSdkInfo: true, workDirectory: 'Botticelli', charset: 'Utf8', framework: 'net7.0', nologo: true)
        dotnetPublish(charset: 'utf8', framework: 'net7.0', options: '-r linux-x64 --self-contained false', outputDirectory: 'publish', force: true, workDirectory: 'Botticelli')
        sh 'scp -pw \'12345678\' -C -rp  publish agent@45.126.125.65:/deploy/server_back'
      }
    }

  }
}