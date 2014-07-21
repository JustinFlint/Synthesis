#include "StateNetworkServer.h"
#include "OSAL/OSAL.h"
#include <stdio.h>
#if USE_WINAPI
#include <Windows.h>
#elif USE_POSIX
#include <sys/socket.h>
#endif

#include <string.h>

StateNetworkServer::StateNetworkServer(void)
{
}


StateNetworkServer::~StateNetworkServer(void)
{
}

void StateNetworkServer::Open() {
#if USE_WINAPI
	WSADATA wsa;
	WSAStartup(MAKEWORD(2,2),&wsa);		// Hope and pray that this works.
#endif
	udpSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (udpSocket < 0) {
		fprintf(stderr, "Could not create socket!\n");
		exit(2);
	}
	struct sockaddr_in server;
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = INADDR_ANY;
	server.sin_port = htons( PORT );

	if (bind(udpSocket, (struct sockaddr *)&server, sizeof(server)) == SOCKET_ERROR) {
		fprintf(stderr, "Could not bind socket!\n");
		exit(2);
	}

	// Who cares about data!  We don't need to listen!
}

void StateNetworkServer::Close() {
	closesocket(udpSocket);
#if USE_WINAPI
	WSACleanup();
#endif
}

void StateNetworkServer::SendStatePacket(StatePacket pack) {
	struct sockaddr_in server;
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = inet_addr("127.0.0.1");
	server.sin_port = htons( PORT );
	sendto(udpSocket, (char*) &pack, sizeof(pack), 0, (sockaddr*) &server, sizeof(server));
}