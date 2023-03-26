/**
 * @file include/glShader.hpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#pragma once

#include <string>

#include <GL/glew.h>

#include "disposable.hpp"

namespace glekcraft {
    class GLShader : public Disposable {
    private:
        /**
         * The internal OpenGL ID of the instance.
         */
        GLuint m_id;

        /**
         * The type of the instance.
         */
        GLenum m_type;

        /**
         * Whether the instance has been disposed.
         */
        bool m_disposed;

        /**
         * The copy constructor.
         *
         * @param other The other instance to copy.
         */
        GLShader(const GLShader&) = delete;

        /**
         * The copy assignment operator.
         *
         * @param other The other instance to copy.
         *
         * @returns A copy of this instance.
         */
        GLShader& operator=(const GLShader&) = delete;

    public:
        /**
         * Create a new instance.
         *
         * @param type The type of instance to create.
         */
        GLShader(GLenum type);

        /**
         * The finalizer.
         */
        ~GLShader();

        /**
         * Get the internal OpenGL ID of the instance.
         *
         * @returns The internal OpenGL ID of the instance.
         */
        GLuint GetID() const;

        /**
         * Check whether the instance has been disposed.
         *
         * @returns Whether the instance has been disposed.
         *
         * @see Dispose
         */
        bool IsDisposed() const override;

        /**
         * Check whether the instance has been compiled.
         *
         * @returns Whether the instance has been compiled.
         */
        bool IsCompiled() const;

        /**
         * Get the info log of the instance.
         *
         * @returns The info log of the instance.
         */
        std::string GetInfoLog() const;

        /**
         * Dispose the instance.
         *
         * @see IsDisposed
         */
        void Dispose() override;

        /**
         * Upload shader source code into the instance..
         *
         * @param source The source code to upload into the instance.
         */
        void UploadSource(const char* source);

        /**
         * Upload shader source code into the instance..
         *
         * @param source The source code to upload into the instance.
         */
        void UploadSource(const std::string& source);

        /**
         * Attempt to compile the instance.
         */
        void Compile();

    }; // class GLShader
} // namespace glekcraft
