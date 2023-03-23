/**
 * @file src/game.cpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#include "game.hpp"

#include <stdexcept>

namespace glekcraft {
    Game::Game() {
        m_initialized = false;
        m_running = false;
        m_exitCode = EXIT_SUCCESS;
        m_shouldExit = false;
        m_window = nullptr;
    }

    Game::~Game() {
        Terminate();
    }

    bool Game::IsInitialized() const {
        return m_initialized;
    }

    bool Game::IsRunning() const {
        return m_running;
    }

    int Game::GetExitCode() const {
        return m_exitCode;
    }

    void Game::SetExitCode(int value) {
        m_exitCode = value;
    }

    bool Game::ShouldExit() const {
        return m_shouldExit;
    }

    void Game::SetShouldExit(bool value) {
        m_shouldExit = value;
    }

    void Game::RequestExit(int exitCode) {
        SetExitCode(exitCode);
        SetShouldExit(true);
    }

    void Game::Initialize() {
        if (IsInitialized()) {
            throw std::logic_error("Game instance already initialized");
        }
        if (!glfwInit()) {
            throw std::runtime_error("Failed to initialize GLFW");
        }
        glfwWindowHint(GLFW_RESIZABLE, true);
        glfwWindowHint(GLFW_VISIBLE, false);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 5);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
        glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, true);
        m_window = glfwCreateWindow(640, 480, "Glekcraft", nullptr, nullptr);
        if (m_window == nullptr) {
            throw std::runtime_error("Failed to create game window");
        }
        glfwMakeContextCurrent(m_window);
        m_initialized = true;
    }

    void Game::Run() {
        if (!IsInitialized()) {
            throw std::logic_error("Game instance not initialized");
        }
        if (IsRunning()) {
            throw std::logic_error("Game instance already running");
        }
        glfwShowWindow(m_window);
        m_running = true;
        while (!ShouldExit()) {
            Update();
            Render();
        }
        m_running = false;
    }

    void Game::Terminate() noexcept {
        m_initialized = false;
        if (m_window != nullptr) {
            glfwDestroyWindow(m_window);
            m_window = nullptr;
        }
        glfwTerminate();
    }

    void Game::Update() {
        glfwPollEvents();
        if (glfwWindowShouldClose(m_window)) {
            RequestExit(EXIT_SUCCESS);
            glfwSetWindowShouldClose(m_window, false);
        }
        if (glfwGetKey(m_window, GLFW_KEY_ESCAPE)) {
            RequestExit(EXIT_SUCCESS);
        }
        // TODO
    }

    void Game::Render() {
        glfwMakeContextCurrent(m_window);
        // TODO
        glfwSwapBuffers(m_window);
    }
} // namespace glekcraft
