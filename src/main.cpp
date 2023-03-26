/**
 * @file src/main.cpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#include <cstdlib>
#include <vector>

#include <spdlog/spdlog.h>
#include <spdlog/sinks/rotating_file_sink.h>
#include <spdlog/sinks/stdout_color_sinks.h>

#ifdef DEBUG
#include <spdlog/sinks/msvc_sink.h>
#endif

#include "game.hpp"

void setupLogging() {
    auto sinks = std::vector<spdlog::sink_ptr>();
    sinks.push_back(std::make_shared<spdlog::sinks::stdout_color_sink_st>());
    sinks.push_back(std::make_shared<spdlog::sinks::rotating_file_sink_st>(
        "output.log", 1024 * 1024, 3));
#ifdef DEBUG
    sinks.push_back(std::make_shared<spdlog::sinks::msvc_sink_st>());
#endif
    auto logger =
        std::make_shared<spdlog::logger>("root", sinks.begin(), sinks.end());
    spdlog::set_default_logger(logger);
    spdlog::set_level(spdlog::level::info);
    // TODO: Maybe add timestamp to pattern?
    spdlog::set_pattern("[%^%l%$@%n] %v");
    spdlog::info("Logging initialized");
}

int main(int argc, char** argv) {
    setupLogging();
    auto exitCode = EXIT_SUCCESS;
    auto game = glekcraft::Game();
    try {
        game.Initialize();
        game.Run();
        exitCode = game.GetExitCode();
    } catch (const std::exception& e) {
        spdlog::critical("Unhandled exception: {}", e.what());
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
