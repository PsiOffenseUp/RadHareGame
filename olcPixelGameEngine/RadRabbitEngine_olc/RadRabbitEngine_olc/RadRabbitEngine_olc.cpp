#include "RadRabbitEngine.h"

class Game : public olc::PixelGameEngine {
public:

	bool OnUserCreate() override {
		sAppName = "Rad Hare Game";

		return true;
	}

	bool OnUserUpdate(float fElapsedTime) override {

		return true;
	}
};

int main()
{
	Game game;

	if (game.Construct(800, 600, 1, 1)) {
		game.Start();
	}
}
