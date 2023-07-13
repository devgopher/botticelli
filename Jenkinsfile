def remote = [:]
    remote.name = 'test_node1'
    remote.host = '45.126.125.65'
    remote.user = 'agent'
    remote.password = '12345678'
    remote.allowAnyHosts = true


pipeline {
  agent any
  stages {
    stage('back_publish_dev') {
      steps {
        dotnetClean(showSdkInfo: true, workDirectory: 'Botticelli', charset: 'Utf8', framework: 'net7.0', nologo: true)
        sh ''' dotnet publish Botticelli -o publish -r linux-x64 --self-contained false
'''
      }
    }

    stage('publish_to_node1') {
      steps {
        sh '''sshpass -p \'12345678\' scp  -C -rp publish agent@45.126.125.65:/deploy/server_back
'''
      }
    }

    stage('run on node 1') {
      steps {
        sshCommand(command: 'pkill -f Botticelli.Server', remote: remote)
        sshCommand(command: ' cd /deploy/server_back/ | dotnet run', remote: remote)
      }
    }

  }
}