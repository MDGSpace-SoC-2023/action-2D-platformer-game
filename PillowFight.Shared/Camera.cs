using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PillowFight.Shared;

public sealed class Camera
{
    private Matrix _transformationMatrix;
    private Matrix _inverseMatrix;
    private Vector2 _position;
    private float _rotation;
    private Vector2 _zoom;
    private Vector2 _origin;
    private bool _hasChanged;
    public Viewport Viewport { get; set; }
    public Matrix TransformationMatrix {
        get {
            if (_hasChanged) {
                UpdateMatrices();
            }

            return _transformationMatrix;
        }
    }

    public Matrix InverseMatrix {
        get {
            if (_hasChanged) {
                UpdateMatrices();
            }

            return _inverseMatrix;
        }
    }

    public Vector2 Position {
        get { return _position; }
        set {
            if (_position == value) { return; }

            _position = value;
            _hasChanged = true;
        }
    }

    
    public float X {
        get { return _position.X; }
        set {
            if (_position.X == value) { return; }

            _position.X = value;
            _hasChanged = true;
        }
    }

    
    public float Y {
        get { return _position.Y; }
        set {
            if (_position.Y == value) { return; }

            _position.Y = value;
            _hasChanged = true;
        }
    }

    public float Rotation {
        get { return _rotation; }
        set {
            if (_rotation == value) { return; }

            _rotation = value;
            _hasChanged = true;
        }
    }

    public Vector2 Zoom
    {
        get { return _zoom; }
        set {
            if (_zoom == value) { return; }

            _zoom = value;
            _hasChanged = true;
        }
    }

    public Vector2 Origin {
        get { return _origin; }
        set {
            if (_origin == value) { return; }

            _origin = value;
            _hasChanged = true;
        }
    }

    public Camera(int width, int height)
        : this(new Viewport(0, 0, width, height, 0, 1)) { }

    public Camera(Viewport viewport) {
        _position = Vector2.Zero;
        _rotation = 0.0f;
        _origin = Vector2.Zero;
        _zoom = Vector2.One;

        Viewport = viewport;
        UpdateMatrices();
    }

    private void UpdateMatrices() {
        Matrix translationMatrix = Matrix.CreateTranslation(new Vector3 {
            X = -(int)Math.Floor(_position.X),
            Y = -(int)Math.Floor(_position.Y),
            Z = 0
        });

        Matrix rotationMatrix = Matrix.CreateRotationZ(_rotation);

        Matrix scaleMatrix = Matrix.CreateScale(new Vector3 {
            X = _zoom.X,
            Y = _zoom.Y,
            Z = 1
        });

        Matrix originTranslationMatrix = Matrix.CreateTranslation(new Vector3 {
            X = (int)Math.Floor(_origin.X),
            Y = (int)Math.Floor(_origin.Y),
            Z = 0
        });

        _transformationMatrix = Matrix.Identity *
                                translationMatrix *
                                rotationMatrix *
                                scaleMatrix *
                                originTranslationMatrix;

        _inverseMatrix = Matrix.Invert(_transformationMatrix);

        _hasChanged = false;
    }

    public void CenterOrigin() {
        Origin = new Vector2(Viewport.Width, Viewport.Height) * 0.5f;
    }

    public Vector2 ScreenToCamera(Vector2 screenPosition) {
        return Vector2.Transform(screenPosition, InverseMatrix);
    }

    public Vector2 CameraToScreen(Vector2 worldPosition) {
        return Vector2.Transform(worldPosition, TransformationMatrix);
    }
}
