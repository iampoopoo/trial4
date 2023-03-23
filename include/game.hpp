/**
 * @file include/game.hpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#pragma once

#include <cstdlib>

#include <GLFW/glfw3.h>

namespace glekcraft {
    /**
     * The main game logic/state controller.
     */
    class Game {
    private:
        /**
         * Whether the instance has been initialized.
         */
        bool m_initialized;

        /**
         * Whether the instance is running.
         */
        bool m_running;

        /**
         * The exit code to return to the operating system.
         */
        int m_exitCode;

        /**
         * Whether the instance should exit after the current frame.
         */
        bool m_shouldExit;

        /**
         * The main window.
         */
        GLFWwindow* m_window;

        /**
         * The copy constructor.
         *
         * @param other The other instance to copy.
         */
        Game(const Game& other) = delete;

        /**
         * The copy assignment operator.
         *
         * @param other The other instance to copy.
         *
         * @return A copy of the other instance.
         */
        Game& operator=(const Game& other) = delete;

    public:
        /**
         * Create a new instance.
         */
        Game();

        /**
         * The finalizer.
         */
        ~Game();

        /**
         * Check whether the instance has been initialized.
         *
         * @return Whether the instance has been initialized.
         */
        bool IsInitialized() const;

        /**
         * Check whether the instance is running.
         *
         * @return Whether the instance is running.
         */
        bool IsRunning() const;

        /**
         * Get the exit code to return to the operating system.
         *
         * @return The exit code to return to the operating system.
         */
        int GetExitCode() const;

        /**
         * Set the exit code to return to the operating system.
         *
         * @param value The exit code to return to the operating system.
         */
        void SetExitCode(int value);

        /**
         * Check whether the instance should exit after the current frame.
         *
         * @return Whether the instance should exit after the current frame.
         */
        bool ShouldExit() const;

        /**
         * Set whether the instance should exit after the current frame.
         *
         * @param value Whether the instance should exit after the current
         * frame.
         */
        void SetShouldExit(bool value);

        /**
         * Request that the instance exit after the current frame with the given
         * exit code.
         *
         * @param exitCode The exit code to return to the operating system once
         * the instance exits.
         */
        void RequestExit(int exitCode = EXIT_SUCCESS);

        /**
         * Initialize the instance.
         */
        void Initialize();

        /**
         * Run the instance.
         */
        void Run();

        /**
         * Shut down the instance.
         */
        void Terminate() noexcept;

    private:
        /**
         * Update the instance.
         */
        void Update();

        /**
         * Render the instance.
         */
        void Render();
    }; // class Game
} // namespace glekcraft
