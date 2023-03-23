/**
 * @file src/main.cpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#include <cstdlib>
#include <iostream>

#include "game.hpp"

int main(int argc, char** argv) {
    auto exitCode = EXIT_SUCCESS;
    auto game = glekcraft::Game();
    try {
        game.Initialize();
        game.Run();
        exitCode = game.GetExitCode();
    } catch (const std::exception& e) {
        std::cerr << "Fatal Error: " << e.what() << std::endl;
        exitCode = EXIT_FAILURE;
    }
    game.Terminate();
    return exitCode;
}

#ifdef _WIN32
#include <windows.h>

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance,
                   LPSTR lpCmdLine, int nCmdShow) {
    return main(__argc, __argv);
}

#endif
