/**
 * @file include/disposable.hpp
 * @author G'lek Tarssza
 * @copyright Copyright (c) 2023 G'lek Tarssza
 * @copyright All rights reserved.
 */

#pragma once

namespace glekcraft {
    /**
     * The base interface for all disposable objects.
     *
     * @interface
     */
    class Disposable {
    public:
        /**
         * The finalizer.
         */
        virtual ~Disposable() = default;

        /**
         * Check whether the instance has been disposed.
         *
         * @returns Whether the instance has been disposed.
         *
         * @see Dispose
         */
        virtual bool IsDisposed() const = 0;

        /**
         * Dispose the instance.
         *
         * @see IsDisposed
         */
        virtual void Dispose() = 0;
    }; // class Disposable
} // namespace glekcraft
